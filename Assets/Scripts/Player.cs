using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviour, IPunObservable {
    public float walkSpeed;

    Animator anim;
    PhotonView pv;
    Rigidbody2D rb;

    Vector3 curPos;

    void Awake() {
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (pv.IsMine) {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            rb.velocity = new Vector3(x, y).normalized * walkSpeed;

            anim.SetBool("isWalk", rb.velocity != Vector2.zero);
        } else {
            if ((transform.position - curPos).sqrMagnitude >= 100) {
                transform.position = curPos;
            } else {
                transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(transform.position);
        } else {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
}
