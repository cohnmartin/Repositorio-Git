using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    partial class SegMenu
    {
        public long? IdPadre
        {
            get
            {
                if (this.Padre != null)
                {
                    return this.Padre.IdSegMenu;
                }
                return null;
            }
        }

    }
}
