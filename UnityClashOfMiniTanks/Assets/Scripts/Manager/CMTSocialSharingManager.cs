using UnityEngine;
using System.Collections;

public class CMTSocialSharingManager : MonoBehaviour {

	public Texture2D imageToShare; 

	public void ShareFacebook()
	{
		IOSSocialManager.Instance.FacebookPost("Download and play Clash of Mini Tanks. Its free!","https://itunes.apple.com/us/app/clash-of-mini-tanks/id1039703100?ls=1&mt=8",imageToShare);
	}

	public void ShareTwitter()
	{
		IOSSocialManager.Instance.TwitterPost("Download and play Clash of Mini Tanks. Its free!","https://itunes.apple.com/us/app/clash-of-mini-tanks/id1039703100?ls=1&mt=8",imageToShare); 
	}

	public void ShareWhatsApp()
	{
		IOSSocialManager.Instance.WhatsAppShareText("Download and play Clash of Mini Tanks. Its free! https://itunes.apple.com/us/app/clash-of-mini-tanks/id1039703100?ls=1&mt=8"); //
	}
}
