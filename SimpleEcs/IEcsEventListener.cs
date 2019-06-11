using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEcs {

    public interface IEcsEventListener {

        void EneityRemoved(AbstractEntity entity);
    }
}
