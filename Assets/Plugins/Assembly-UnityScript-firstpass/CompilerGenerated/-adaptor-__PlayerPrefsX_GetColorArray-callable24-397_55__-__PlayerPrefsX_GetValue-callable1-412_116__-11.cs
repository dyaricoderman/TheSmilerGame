using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_GetColorArray_0024callable24_0024397_55___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___002411
	{
		protected __PlayerPrefsX_GetColorArray_0024callable24_0024397_55__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_GetColorArray_0024callable24_0024397_55___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___002411(__PlayerPrefsX_GetColorArray_0024callable24_0024397_55__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1)
		{
			_0024from((List<Color>)arg0, arg1);
		}

		public static __PlayerPrefsX_GetValue_0024callable1_0024412_116__ Adapt(__PlayerPrefsX_GetColorArray_0024callable24_0024397_55__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_GetColorArray_0024callable24_0024397_55___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___002411(from).Invoke;
		}
	}
}
