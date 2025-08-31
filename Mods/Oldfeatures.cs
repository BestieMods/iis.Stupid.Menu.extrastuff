using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using GorillaLocomotion;

namespace Mods.Oldfeatures
{
    public class SnowballCrashGun : MonoBehaviour
    {
        private float cooldown = 5f;
        private float lastCrashTime = -5f;

        public void RegisterButton()
        {
            new Button("Snowball Crash", () =>
            {
                if (Time.time - lastCrashTime < cooldown)
                    return;

                Player target = GetTargetedPlayer();
                if (target != null)
                {
                    byte crashEventCode = 199; // Safest known custom event code
                    object[] payload = new object[]
                    {
                        "SNOWBALL_CRASH_" + UnityEngine.Random.Range(1000, 9999),
                        target.ActorNumber
                    };

                    RaiseEventOptions options = new RaiseEventOptions
                    {
                        TargetActors = new int[] { target.ActorNumber },
                        Receivers = ReceiverGroup.Others
                    };

                    PhotonNetwork.RaiseEvent(crashEventCode, payload, options, SendOptions.SendReliable);
                    lastCrashTime = Time.time;

                    PlaySnowballEffect(target);
                }
            });
        }

        private Player GetTargetedPlayer()
        {
            foreach (Player p in PhotonNetwork.PlayerListOthers)
            {
                Vector3 localPos = GorillaLocomotion.Player.Instance.transform.position;
                Vector3 targetPos = p.GetPosition(); // Requires extension method or mod hook
                if (Vector3.Distance(localPos, targetPos) < 5f)
                {
                    return p;
                }
            }
            return null;
        }

        private void PlaySnowballEffect(Player target)
        {
            Vector3 pos = target.GetPosition(); // Same note as above
            GameObject snowPuff = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            snowPuff.transform.position = pos;
            snowPuff.transform.localScale = Vector3.one * 0.5f;
            snowPuff.GetComponent<Renderer>().material.color = Color.white;
            Destroy(snowPuff, 1f);
        }
    }
}
