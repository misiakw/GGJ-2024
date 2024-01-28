using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using static Assets.KinectGame.Enums;

public class ClownController3 : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject LeftLeg;
    public GameObject RightLeg;

    public GameObject BodySourceManager;
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    protected Transform bodyRoot;
    // A required variable if you want to rotate the model in space.
    protected GameObject offsetNode;

    // Rotations of the bones when the Kinect tracking starts.
    protected Quaternion[] initialRotations;
    protected Quaternion[] initialLocalRotations;

    protected Transform[] bones;

    protected virtual void MapBones()
    {
        // make OffsetNode as a parent of model transform.
        //offsetNode = new GameObject(name + "Ctrl") { layer = transform.gameObject.layer, tag = transform.gameObject.tag };
        //offsetNode.transform.position = transform.position;
        //offsetNode.transform.rotation = transform.rotation;
        //offsetNode.transform.parent = transform.parent;

        //transform.parent = offsetNode.transform;
        //transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity;

        // take model transform as body root
        bodyRoot = transform;

        for(int i = 0; i!= supportedTypes.Count; i++)
        {
            var supportedType = supportedTypes[i];
            switch (supportedType)
            {
                case JointType.HandLeft:
                    bones[i] = LeftHand.transform;
                    break;
                case JointType.HandRight:
                    bones[i] = RightHand.transform;
                    break;
                case JointType.AnkleLeft:
                    bones[i] = LeftLeg.transform;
                    break;
                case JointType.AnkleRight:
                    bones[i] = RightLeg.transform;
                    break;
            }

            //Limb newLimb = bones[(int)jt].gameObject.AddComponent<Limb>();
            //switch (jt)
            //{
            //        case JointType.HandLeft:
            //            newLimb.LimbType = LimbType.LeftHand;
            //            break;
            //        case JointType.HandRight:
            //            newLimb.LimbType = LimbType.RightHand;
            //            break;
            //        case JointType.FootLeft:
            //            newLimb.LimbType = LimbType.LeftFoot;
            //            break;
            //        case JointType.FootRight:
            //            newLimb.LimbType = LimbType.RightFoot;
            //            break;
            //    }
            //}
        }
    }
    public void Awake()
    {
        // check for double start
        if (bones != null)
            return;

        // inits the bones array
        bones = new Transform[4];

        // Initial rotations and directions of the bones.
        initialRotations = new Quaternion[bones.Length];
        initialLocalRotations = new Quaternion[bones.Length];

        // Map bones to the points the Kinect tracks
        MapBones();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BodySourceManager == null)
        {
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }

        Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                //MoveAvatar(1);
                MoveAvatar(body);
            }
        }

        return;
    }

    private static List<JointType> supportedTypes = new List<JointType>() { JointType.HandLeft, JointType.HandRight, JointType.AnkleLeft, JointType.AnkleRight };

    private void MoveAvatar(Body body)
    {
        foreach (var supportedType in supportedTypes)
        {
            Windows.Kinect.Joint sourceJoint = body.Joints[supportedType];
            Windows.Kinect.Joint originJoint = body.Joints[supportedType];
            Vector3 translatedRotations = new Vector3();
            switch (supportedType)
            {
                case JointType.HandLeft:
                    originJoint = body.Joints[JointType.ShoulderLeft];
                    double a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    double b = a;
                    double c = CalculateLength(originJoint.Position.X - a, sourceJoint.Position.X, originJoint.Position.Y, sourceJoint.Position.Y);
                    double newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if(sourceJoint.Position.Y > originJoint.Position.Y)
                    {
                        newZRotation *= -1;
                    }
                    LeftHand.transform.localEulerAngles = new Vector3(LeftHand.transform.localEulerAngles.x, LeftHand.transform.localEulerAngles.y, (float)newZRotation);
                    break;
                case JointType.HandRight:
                    originJoint = body.Joints[JointType.ShoulderRight];
                    a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    b = a;
                    c = CalculateLength(originJoint.Position.X + a, sourceJoint.Position.X, originJoint.Position.Y, sourceJoint.Position.Y);
                    newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.Y < originJoint.Position.Y)
                    {
                        newZRotation *= -1;
                    }
                    RightHand.transform.localEulerAngles = new Vector3(LeftHand.transform.localEulerAngles.x, LeftHand.transform.localEulerAngles.y, (float)newZRotation);
                    break;
                case JointType.AnkleLeft:
                    originJoint = body.Joints[JointType.HipLeft];
                    a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    b = a;
                    c = CalculateLength(originJoint.Position.X, sourceJoint.Position.X, originJoint.Position.Y - a, sourceJoint.Position.Y);
                    newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.X < originJoint.Position.X)
                    {
                        newZRotation *= -1;
                    }
                    LeftLeg.transform.localEulerAngles = new Vector3(LeftHand.transform.localEulerAngles.x, LeftHand.transform.localEulerAngles.y, (float)newZRotation);
                    break;
                case JointType.AnkleRight:
                    originJoint = body.Joints[JointType.HipRight];
                    a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    b = a;
                    c = CalculateLength(originJoint.Position.X, sourceJoint.Position.X, originJoint.Position.Y - a, sourceJoint.Position.Y);
                    newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.X > originJoint.Position.X)
                    {
                        newZRotation *= -1;
                    }
                    RightLeg.transform.localEulerAngles = new Vector3(LeftHand.transform.localEulerAngles.x, LeftHand.transform.localEulerAngles.y, (float)newZRotation);
                    break;
            }

            

            
            //var boneToEdit = bones[(int)supportedType];
            //boneToEdit.position = (GetVector3FromJoint(sourceJoint) - GetVector3FromJoint(body.Joints[JointType.SpineBase])) * 3f - new Vector3(0, 2);
        }
    }

    static double CalculateAngle(double a, double b, double c)
    {
        var result = (a * a + b * b - c * c) / (2 * a * b);
        return Math.Acos(result);
    }

    static double CalculateLength(double x1, double x2, double y1, double y2)
    {
        return Math.Sqrt(Math.Abs(x1 - x2)* Math.Abs(x1 - x2) + Math.Abs(y1 - y2)* Math.Abs(y1 - y2));
    }

    private static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, 0);
    }
}
