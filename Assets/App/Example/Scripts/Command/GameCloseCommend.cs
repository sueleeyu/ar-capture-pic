using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkDesign.Example
{
    public class GameCloseCommand : ICommand
    {
        public void Excute()
        {
            GameCloseEvent.Trigger();
        }

    }
}
