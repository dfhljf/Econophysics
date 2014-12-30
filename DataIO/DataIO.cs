using System;
using System.Collections.Generic;
using System.Text;

namespace DataIO
{
    interface IDataIO
	{
        public void Write(object obj);
        public object Read();
	}
}
