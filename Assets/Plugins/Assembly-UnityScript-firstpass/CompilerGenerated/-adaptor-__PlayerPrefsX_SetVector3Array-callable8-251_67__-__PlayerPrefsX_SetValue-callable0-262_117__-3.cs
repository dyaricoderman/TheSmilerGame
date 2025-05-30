using System;
using System.Collections;
using UnityEngine;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_SetVector3Array_0024callable8_0024251_67___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00243
	{
		protected __PlayerPrefsX_SetVector3Array_0024callable8_0024251_67__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_SetVector3Array_0024callable8_0024251_67___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00243(__PlayerPrefsX_SetVector3Array_0024callable8_0024251_67__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1, int arg2)
		{
			_0024from((Vector3[])arg0, arg1, arg2);
		}

		public static __PlayerPrefsX_SetValue_0024callable0_0024262_117__ Adapt(__PlayerPrefsX_SetVector3Array_0024callable8_0024251_67__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_SetVector3Array_0024callable8_0024251_67___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00243(from).Invoke;
		}
	}
}
