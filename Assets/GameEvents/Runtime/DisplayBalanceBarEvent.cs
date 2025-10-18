using System.Threading;
using Core.EventsBus;
using UnityEngine;

namespace GameEvents.Runtime
{
    public class DisplayBalanceBarEvent : IBusEvent
    {
        public CancellationToken DisplayToken { get; set; }
        public Transform Parent { get; set; }
    }
}