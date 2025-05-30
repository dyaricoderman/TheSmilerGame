using System;
using System.Collections;
using System.Collections.Generic;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_GetIntArray_0024callable14_0024312_53___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00246
	{
		protected __PlayerPrefsX_GetIntArray_0024callable14_0024312_53__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_GetIntArray_0024callable14_0024312_53___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00246(__PlayerPrefsX_GetIntArray_0024callable14_0024312_53__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1)
		{
			_0024from((List<int>)arg0, arg1);
		}

		public static __PlayerPrefsX_GetValue_0024callable1_0024412_116__ Adapt(__PlayerPrefsX_GetIntArray_0024callable14_0024312_53__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_GetIntArray_0024callable14_0024312_53___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00246(from).Invoke;
		}
	}
}
