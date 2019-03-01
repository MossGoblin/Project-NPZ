using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    interface IAgent
    {
        void Init();
        void Move(float horizontal);
        void Jump();
        void AttackRange();
        void AttackMelee();
        void OnGround();
    }
}
