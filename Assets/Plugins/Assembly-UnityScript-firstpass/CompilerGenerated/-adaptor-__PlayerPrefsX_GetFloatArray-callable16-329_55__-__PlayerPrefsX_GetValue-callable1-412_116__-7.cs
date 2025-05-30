using System;
using System.Collections;
using System.Collections.Generic;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_GetFloatArray_0024callable16_0024329_55___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00247
	{
		protected __PlayerPrefsX_GetFloatArray_0024callable16_0024329_55__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_GetFloatArray_0024callable16_0024329_55___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00247(__PlayerPrefsX_GetFloatArray_0024callable16_0024329_55__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1)
		{
			_0024from((List<float>)arg0, arg1);
		}

		public static __PlayerPrefsX_GetValue_0024callable1_0024412_116__ Adapt(__PlayerPrefsX_GetFloatArray_0024callable16_0024329_55__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_GetFloatArray_0024callable16_0024329_55___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00247(from).Invoke;
		}
	}
}
