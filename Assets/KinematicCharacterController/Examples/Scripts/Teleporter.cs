using UnityEngine;
using UnityEngine.Events;

namespace KinematicCharacterController.Examples
{
    public class Teleporter : MonoBehaviour
    {
        public Teleporter TeleportTo;

        public UnityAction<ExampleCharacterController> OnCharacterTeleport;

        private bool _canTeleport = true;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Something entered: " + other.name);

            if (!_canTeleport)
                return;

            CharacterController player = other.GetComponent<CharacterController>();

            if (player == null)
            {
                Debug.Log("No CharacterController found!");
                return;
            }

            if (TeleportTo == null)
            {
                Debug.Log("No TeleportTo assigned!");
                return;
            }

            TeleportTo._canTeleport = false;

            player.enabled = false;

            player.transform.position = TeleportTo.transform.position;
            player.transform.rotation = TeleportTo.transform.rotation;

            player.enabled = true;

            Debug.Log("TELEPORTED!");

        }

        private void OnTriggerExit(Collider other)
        {
            _canTeleport = true;

            if (TeleportTo != null)
            {
                TeleportTo._canTeleport = true;
            }
        }
    }
}