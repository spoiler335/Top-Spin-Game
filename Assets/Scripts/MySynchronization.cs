using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySynchronization : MonoBehaviour,IPunObservable
{
    Rigidbody rb;
    PhotonView PhotonView;

    Vector3 networkPosition;
    Quaternion netwrokRotation;

    public bool syncVelociy = true;
    public bool syncAngularVelociy = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private float distance;
    private float angle;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PhotonView = GetComponent<PhotonView>();
        networkPosition = new Vector3();
        netwrokRotation = new Quaternion();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!PhotonView.IsMine)
        {
            rb.position = Vector3.MoveTowards(rb.position, networkPosition, distance*(1.0f/PhotonNetwork.SerializationRate) );
            rb.rotation = Quaternion.RotateTowards(rb.rotation, netwrokRotation, angle* (1.0f / PhotonNetwork.SerializationRate));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);

            if(syncVelociy)
            {
                stream.SendNext(rb.velocity);
            }
            if(syncAngularVelociy)
            {
                stream.SendNext(rb.angularVelocity);
            }
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            netwrokRotation = (Quaternion)stream.ReceiveNext();

            if(isTeleportEnabled)
            {
                if( Vector3.Distance(rb.position,networkPosition)>teleportIfDistanceGreaterThan  )
                {
                    rb.position = networkPosition;
                }
            }

            if(syncVelociy || syncAngularVelociy)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if(syncVelociy)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();

                    networkPosition += rb.velocity*lag;

                    distance = Vector3.Distance(rb.position, networkPosition);
                }

                if(syncAngularVelociy)
                {
                    rb.angularVelocity = (Vector3)(stream.ReceiveNext());
                    netwrokRotation = Quaternion.Euler(rb.angularVelocity * lag)*netwrokRotation;
                    angle = Quaternion.Angle(rb.rotation, netwrokRotation);
                }
            }
        }
    }
}
