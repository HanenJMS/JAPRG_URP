using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLab.AI
{
    internal class CommandAI : MonoBehaviour, IAIState
    {
        private void LateUpdate()
        {
            
        }
        public void ActivateState()
        {
            throw new NotImplementedException();
        }

        public void CancelState()
        {
            throw new NotImplementedException();
        }

        public bool IsRunning()
        {
            throw new NotImplementedException();
        }
    }
}
