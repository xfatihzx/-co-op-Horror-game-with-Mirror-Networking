using UnityEngine;

namespace Depracteds
{
    public class ActionDebug : InterectAction
    {
        public override void Action()
        {
            Debug.Log("Wow. Interact. Great!");
        }
    }
}