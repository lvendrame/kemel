﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemel.Kuery
{
    public abstract class ProviderContainer<T, E>
    {
        private T _provider;

        public T Provider
        {
            get { return _provider; }
        }

        public ProviderContainer(T provider)
        {
            this._provider = provider;
        }

        public abstract void DoBehavior(E enumType);
    }
}
