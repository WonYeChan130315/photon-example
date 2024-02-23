using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class NetworkManager : MonoBehaviourPunCallbacks {
    public TMP_InputField roomInput;
    public TextMeshProUGUI loadingTxt;

    public GameObject lobbyGroup;
    public GameObject loadingGroup;

    bool joined = false;
    bool connected = false;

    public void Join() {
        if (connected) return;
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 10 }, null);

        connected = true;

        lobbyGroup.SetActive(false);
        loadingGroup.SetActive(true);

        StartCoroutine("BlinkText");
    }

    IEnumerator BlinkText() {
        float hue = 0f;
        float saturation = 1f;
        float value = 1f;

        while (!joined) {
            Color color = Color.HSVToRGB(hue, saturation, value);
            loadingTxt.color = color;
            hue = (hue + 0.05f) % 1f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public override void OnJoinedRoom() {
        loadingGroup.SetActive(false);
        joined = true;

        PhotonNetwork.Instantiate("Player", Vector2.zero, Quaternion.identity);
    }
}
