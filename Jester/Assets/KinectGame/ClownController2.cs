using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Kinect = Windows.Kinect;

public class ClownController2 : MonoBehaviour
{
    public GameObject BodySourceManager;
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    // private instance of the KinectManager
    protected KinectManager kinectManager;


    // Bool that has the characters (facing the player) actions become mirrored. Default false.
    public bool mirroredMovement = false;

    // Bool that determines whether the avatar is allowed to move in vertical direction.
    public bool verticalMovement = false;

    // Rate at which avatar will move through the scene. The rate multiplies the movement speed (.001f, i.e dividing by 1000, unity's framerate).
    protected int moveRate = 1;

    // Slerp smooth factor
    public float smoothFactor = 5f;

    // Whether the offset node must be repositioned to the user's coordinates, as reported by the sensor or not.
    public bool offsetRelativeToSensor = false;


    // The body root node
    protected Transform bodyRoot;

    // A required variable if you want to rotate the model in space.
    protected GameObject offsetNode;

    // Calibration Offset Variables for Character Position.
    protected bool offsetCalibrated = false;
    protected float xOffset, yOffset, zOffset;

    // Variable to hold all them bones. It will initialize the same size as initialRotations.
    protected Transform[] bones;

    // Rotations of the bones when the Kinect tracking starts.
    protected Quaternion[] initialRotations;
    protected Quaternion[] initialLocalRotations;

    // dictionaries to speed up bones' processing
    // the author of the terrific idea for kinect-joints to mecanim-bones mapping
    // along with its initial implementation, including following dictionary is
    // Mikhail Korchun (korchoon@gmail.com). Big thanks to this guy!
    private readonly Dictionary<JointType, HumanBodyBones> jointToHumanBody = new Dictionary<JointType, HumanBodyBones>
    {
        {JointType.SpineBase, HumanBodyBones.Hips},
        {JointType.SpineMid, HumanBodyBones.Spine},
        {JointType.Neck, HumanBodyBones.Neck},
        {JointType.Head, HumanBodyBones.Head},

        {JointType.ShoulderLeft, HumanBodyBones.LeftShoulder},
        {JointType.ElbowLeft, HumanBodyBones.LeftUpperArm},
        {JointType.WristLeft, HumanBodyBones.LeftLowerArm},
        {JointType.HandLeft, HumanBodyBones.LeftHand},
        {JointType.HandTipLeft, HumanBodyBones.LeftIndexProximal},

        {JointType.ShoulderRight, HumanBodyBones.RightShoulder},
        {JointType.ElbowRight, HumanBodyBones.RightUpperArm},
        {JointType.WristRight, HumanBodyBones.RightLowerArm},
        {JointType.HandRight, HumanBodyBones.RightHand},
        {JointType.HandTipRight, HumanBodyBones.RightIndexProximal},

        {JointType.HipLeft, HumanBodyBones.LeftUpperLeg},
        {JointType.KneeLeft, HumanBodyBones.LeftLowerLeg},
        {JointType.AnkleLeft, HumanBodyBones.LeftFoot},
        {JointType.FootLeft, HumanBodyBones.LeftToes},

        {JointType.HipRight, HumanBodyBones.RightUpperLeg},
        {JointType.KneeRight, HumanBodyBones.RightLowerLeg},
        {JointType.AnkleRight, HumanBodyBones.RightFoot},
        {JointType.FootRight, HumanBodyBones.RightToes},
    };

    private readonly Dictionary<HumanBodyBones, Tuple<JointType, JointType>> humanBodyFromToJoint = new Dictionary<HumanBodyBones, Tuple<JointType, JointType>>
    {
        {HumanBodyBones.Head, new Tuple<JointType, JointType>(JointType.Neck, JointType.Head)},
        {HumanBodyBones.Neck, new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.Neck)},
        {HumanBodyBones.Chest, new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineShoulder)},
        {HumanBodyBones.Spine, new Tuple<JointType, JointType>(JointType.SpineBase, JointType.SpineMid)},

        {HumanBodyBones.LeftShoulder, new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft)},
        {HumanBodyBones.LeftUpperArm, new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft)},
        {HumanBodyBones.LeftLowerArm, new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft)},
        {HumanBodyBones.LeftHand, new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft)},

        {HumanBodyBones.RightShoulder, new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight)},
        {HumanBodyBones.RightUpperArm, new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight)},
        {HumanBodyBones.RightLowerArm, new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight)},
        {HumanBodyBones.RightHand, new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight)},

        {HumanBodyBones.Hips, new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft)},
        {HumanBodyBones.LeftUpperLeg, new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft)},
        {HumanBodyBones.LeftLowerLeg, new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft)},
        {HumanBodyBones.LeftFoot, new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft)},

        //{HumanBodyBones.Hips, new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight)},
        {HumanBodyBones.RightUpperLeg, new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight)},
        {HumanBodyBones.RightLowerLeg, new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight)},
        {HumanBodyBones.RightFoot, new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight)},
    };



    // dictionaries to speed up bones' processing
    // the author of the terrific idea for kinect-joints to mecanim-bones mapping
    // along with its initial implementation, including following dictionary is
    // Mikhail Korchun (korchoon@gmail.com). Big thanks to this guy!
    private readonly Dictionary<int, HumanBodyBones> boneIndex2MecanimMap = new Dictionary<int, HumanBodyBones>
    {
        {0, HumanBodyBones.Hips},
        {1, HumanBodyBones.Spine},
        {2, HumanBodyBones.Neck},
        {3, HumanBodyBones.Head},

        {4, HumanBodyBones.LeftShoulder},
        {5, HumanBodyBones.LeftUpperArm},
        {6, HumanBodyBones.LeftLowerArm},
        {7, HumanBodyBones.LeftHand},
        {8, HumanBodyBones.LeftIndexProximal},

        {9, HumanBodyBones.RightShoulder},
        {10, HumanBodyBones.RightUpperArm},
        {11, HumanBodyBones.RightLowerArm},
        {12, HumanBodyBones.RightHand},
        {13, HumanBodyBones.RightIndexProximal},

        {14, HumanBodyBones.LeftUpperLeg},
        {15, HumanBodyBones.LeftLowerLeg},
        {16, HumanBodyBones.LeftFoot},
        {17, HumanBodyBones.LeftToes},

        {18, HumanBodyBones.RightUpperLeg},
        {19, HumanBodyBones.RightLowerLeg},
        {20, HumanBodyBones.RightFoot},
        {21, HumanBodyBones.RightToes},
    };

    protected readonly Dictionary<int, JointType> boneIndex2JointMap = new Dictionary<int, JointType>
    {
        {0, JointType.SpineBase},
        {1, JointType.SpineMid},
        {2, JointType.SpineShoulder},
        {3, JointType.Head},

        {5, JointType.ShoulderLeft},
        {6, JointType.ElbowLeft},
        {7, JointType.WristLeft},
        {8, JointType.HandLeft},

        {10, JointType.ShoulderRight},
        {11, JointType.ElbowRight},
        {12, JointType.WristRight},
        {13, JointType.HandRight},

        {14, JointType.HipLeft},
        {15, JointType.KneeLeft},
        {16, JointType.AnkleLeft},
        {17, JointType.FootLeft},

        {18, JointType.HipRight},
        {19, JointType.KneeRight},
        {20, JointType.AnkleRight},
        {21, JointType.FootRight},
    };

    protected readonly Dictionary<int, List<JointType>> specIndex2JointMap = new Dictionary<int, List<JointType>>
    {
        {4, new List<JointType> {JointType.ShoulderLeft, JointType.SpineShoulder } },
        {9, new List<JointType> {JointType.ShoulderRight, JointType.SpineShoulder } },
    };

    protected readonly Dictionary<int, JointType> boneIndex2MirrorJointMap = new Dictionary<int, JointType>
    {
        {0, JointType.SpineBase},
        {1, JointType.SpineMid},
        {2, JointType.SpineShoulder},
        {3, JointType.Head},

        {5, JointType.ShoulderRight},
        {6, JointType.ElbowRight},
        {7, JointType.WristRight},
        {8, JointType.HandRight},

        {10, JointType.ShoulderLeft},
        {11, JointType.ElbowLeft},
        {12, JointType.WristLeft},
        {13, JointType.HandLeft},

        {14, JointType.HipRight},
        {15, JointType.KneeRight},
        {16, JointType.AnkleRight},
        {17, JointType.FootRight},

        {18, JointType.HipLeft},
        {19, JointType.KneeLeft},
        {20, JointType.AnkleLeft},
        {21, JointType.FootLeft},
    };

    protected readonly Dictionary<int, List<JointType>> specIndex2MirrorJointMap = new Dictionary<int, List<JointType>>
    {
        {4, new List<JointType> {JointType.ShoulderRight, JointType.SpineShoulder} },
        {9, new List<JointType> {JointType.ShoulderLeft, JointType.SpineShoulder} },
    };


    // If the bones to be mapped have been declared, map that bone to the model.
    protected virtual void MapBones()
    {
        // make OffsetNode as a parent of model transform.
        offsetNode = new GameObject(name + "Ctrl") { layer = transform.gameObject.layer, tag = transform.gameObject.tag };
        offsetNode.transform.position = transform.position;
        offsetNode.transform.rotation = transform.rotation;
        offsetNode.transform.parent = transform.parent;

        transform.parent = offsetNode.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // take model transform as body root
        bodyRoot = transform;

        // get bone transforms from the animator component
        var animatorComponent = GetComponent<Animator>();

        //for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
        for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
        {
            if (!jointToHumanBody.ContainsKey(jt))
                continue;

            bones[(int)jt] = animatorComponent.GetBoneTransform(jointToHumanBody[jt]);
        }
    }

    protected void MoveAvatar(uint UserID)
    {
        //if (bodyRoot == null || kinectManager == null)
        //    return;
        //if (!kinectManager.IsJointTracked(UserID, (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter))
        //    return;

        // Get the position of the body and store it.
        Vector3 trans =  kinectManager.GetUserPosition(UserID);

        // If this is the first time we're moving the avatar, set the offset. Otherwise ignore it.
        if (!offsetCalibrated)
        {
            offsetCalibrated = true;

            xOffset = !mirroredMovement ? trans.x * moveRate : -trans.x * moveRate;
            yOffset = trans.y * moveRate;
            zOffset = -trans.z * moveRate;

            if (offsetRelativeToSensor)
            {
                Vector3 cameraPos = Camera.main.transform.position;

                float yRelToAvatar = (offsetNode != null ? offsetNode.transform.position.y : transform.position.y) - cameraPos.y;
                Vector3 relativePos = new Vector3(trans.x * moveRate, yRelToAvatar, trans.z * moveRate);
                Vector3 offsetPos = cameraPos + relativePos;

                if (offsetNode != null)
                {
                    offsetNode.transform.position = offsetPos;
                }
                else
                {
                    transform.position = offsetPos;
                }
            }
        }

        // Smoothly transition to the new position
        Vector3 targetPos = Kinect2AvatarPos(trans, verticalMovement);

        if (smoothFactor != 0f)
            bodyRoot.localPosition = Vector3.Lerp(bodyRoot.localPosition, targetPos, smoothFactor * Time.deltaTime);
        else
            bodyRoot.localPosition = targetPos;
    }

    // Converts Kinect position to avatar skeleton position, depending on initial position, mirroring and move rate
    protected Vector3 Kinect2AvatarPos(Vector3 jointPosition, bool bMoveVertically)
    {
        float xPos;
        float yPos;
        float zPos;

        // If movement is mirrored, reverse it.
        if (!mirroredMovement)
            xPos = jointPosition.x * moveRate - xOffset;
        else
            xPos = -jointPosition.x * moveRate - xOffset;

        yPos = jointPosition.y * moveRate - yOffset;
        zPos = -jointPosition.z * moveRate - zOffset;

        // If we are tracking vertical movement, update the y. Otherwise leave it alone.
        Vector3 avatarJointPos = new Vector3(xPos, bMoveVertically ? yPos : 0f, zPos);

        return avatarJointPos;
    }

    public void Awake()
    {
        // check for double start
        if (bones != null)
            return;

        // inits the bones array
        bones = new Transform[25];

        // Initial rotations and directions of the bones.
        initialRotations = new Quaternion[bones.Length];
        initialLocalRotations = new Quaternion[bones.Length];

        // Map bones to the points the Kinect tracks
        MapBones();

        // Get initial bone rotations
        //GetInitialRotations();

        // Get the KinectManager instance
        if (kinectManager == null)
        {
            kinectManager = KinectManager.Instance;
        }
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

        Kinect.Body[] data = _BodyManager.GetData();
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
                MoveAvatar(body);
            }
        }

        return;
        if (!transform.gameObject.activeInHierarchy)
            return;

        // Get the KinectManager instance
        //if (kinectManager == null)
        //{
        //    kinectManager = KinectManager.Instance;
        //}

        // move the avatar to its Kinect position
        uint UserID = 0;
        MoveAvatar(UserID);

        for (var boneIndex = 0; boneIndex < bones.Length; boneIndex++)
        {
            if (!bones[boneIndex])
                continue;

            if (boneIndex2JointMap.ContainsKey(boneIndex))
            {
                JointType joint = !mirroredMovement ? boneIndex2JointMap[boneIndex] : boneIndex2MirrorJointMap[boneIndex];
                //TransformBone(UserID, joint, boneIndex, !mirroredMovement);
            }
            else if (specIndex2JointMap.ContainsKey(boneIndex))
            {
                // special bones (clavicles)
                List<JointType> alJoints = !mirroredMovement ? specIndex2JointMap[boneIndex] : specIndex2MirrorJointMap[boneIndex];

                if (alJoints.Count >= 2)
                {
                    //Vector3 baseDir = alJoints[0].ToString().EndsWith("Left") ? Vector3.left : Vector3.right;
                    //TransformSpecialBone(UserID, alJoints[0], alJoints[1], boneIndex, baseDir, !mirroredMovement);
                }
            }
        }
    }


    private void MoveAvatar(Kinect.Body body)
    {
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            if(!jointToHumanBody.ContainsKey(jt))
            {
                continue;
            }
            var boneToEdit = bones[(int)jt];
            boneToEdit.position = GetVector3FromJoint(sourceJoint, 3f);
            //if (jt == JointType.ElbowLeft)
            //{
            //    TransformBone(boneToEdit, body, jointToHumanBody[jt]);
            //}

            //targetJoint = body.Joints[boneIndex2JointMap[jt]];

            //Transform jointObj = bodyObject.transform.Find(jt.ToString());
            //jointObj.localPosition = GetVector3FromJoint(sourceJoint, transformScale);

            //LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            //if (targetJoint.HasValue)
            //{
            //    lr.SetPosition(0, jointObj.localPosition);
            //    lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value, transformScale));
            //    lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            //}
            //else
            //{
            //    lr.enabled = false;
            //}
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint, float transformScale)
    {
        return new Vector3(joint.Position.X * transformScale, joint.Position.Y * transformScale, 0);
    }

    void TransformBone(Transform bone, Body body, HumanBodyBones humanBodyBones)
    {
        if(!humanBodyFromToJoint.ContainsKey(humanBodyBones))
        {
            return;
        }
        var tuple = humanBodyFromToJoint[humanBodyBones];
        var sourceJoint = body.Joints[tuple.Item1];
        var targetJoint = body.Joints[tuple.Item2];
        double a = CalculateLength(sourceJoint.Position.X, targetJoint.Position.X, sourceJoint.Position.Y, targetJoint.Position.Y);
        double b = a;
        double c = CalculateLength(Math.Abs(sourceJoint.Position.X - a), targetJoint.Position.X, sourceJoint.Position.Y, targetJoint.Position.Y);
        double angle = CalculateAngle(a, b, c);
        //bone.rotation = new Quaternion((float)angle, bone.rotation.y, bone.rotation.z, bone.rotation.w);
        bone.eulerAngles = new Vector3((float)angle * Mathf.Rad2Deg, 0, 0);
    }

    double CalculateAngle(double a, double b, double c)
    {
        var result = (a * a + b * b - c * c) / (2 * a * b);
        return Math.Acos(result);
    }

    double CalculateLength(double x1, double x2, double y1, double y2)
    {
        return Math.Sqrt(Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
    }

    // Apply the rotations tracked by kinect to the joints.
    //protected void TransformBone(uint userId, JointType joint, int boneIndex, bool flip)
    //{
    //    Transform boneTransform = bones[boneIndex];
    //    if (boneTransform == null || kinectManager == null)
    //        return;

    //    int iJoint = (int)joint;
    //    if (iJoint < 0)
    //        return;

    //    //Get Kinect joint orientation
    //    Quaternion jointRotation = kinectManager.GetJointOrientation(userId, iJoint, flip);
    //    if (jointRotation == Quaternion.identity)
    //        return;

    //    // Smoothly transition to the new rotation
    //    Quaternion newRotation = Kinect2AvatarRot(jointRotation, boneIndex);

    //    if (smoothFactor != 0f)
    //        boneTransform.rotation = Quaternion.Slerp(boneTransform.rotation, newRotation, smoothFactor * Time.deltaTime);
    //    else
    //        boneTransform.rotation = newRotation;
    //}

    // Converts kinect joint rotation to avatar joint rotation, depending on joint initial rotation and offset rotation
    protected Quaternion Kinect2AvatarRot(Quaternion jointRotation, int boneIndex)
    {
        // Apply the new rotation.
        Quaternion newRotation = jointRotation * initialRotations[boneIndex];

        //If an offset node is specified, combine the transform with its
        //orientation to essentially make the skeleton relative to the node
        if (offsetNode != null)
        {
            // Grab the total rotation by adding the Euler and offset's Euler.
            Vector3 totalRotation = newRotation.eulerAngles + offsetNode.transform.rotation.eulerAngles;
            // Grab our new rotation.
            newRotation = Quaternion.Euler(totalRotation);
        }

        return newRotation;
    }
}
