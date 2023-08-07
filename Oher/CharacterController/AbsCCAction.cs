//¿ØÖÆ³éÏóÀà
using UnityEngine;

namespace GodHand.Controller.MyCharacterController
{
    public abstract class AbsCCAction : IAction
    {
        protected CharacterController _cController;
        public AbsCCAction(CharacterController characterController)
        {
            _cController = characterController;
        }
        public abstract void PlayAction();
    }
}