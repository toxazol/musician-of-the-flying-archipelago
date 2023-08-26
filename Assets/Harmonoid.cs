using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harmonoid : MonoBehaviour
{
    private FriendController friendController;
    private bool isFollowing = true;
    // Start is called before the first frame update
    void Start()
    {
        friendController = GetComponent<FriendController>();
    }
    public void OnFollow()
    {
        isFollowing = !isFollowing;
        friendController.enabled = isFollowing;
    }
}
