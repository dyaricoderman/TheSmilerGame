using System;
using System.Collections;
using UnityEngine;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_SetColorArray_0024callable12_0024259_63___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00245
	{
		protected __PlayerPrefsX_SetColorArray_0024callable12_0024259_63__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_SetColorArray_0024callable12_0024259_63___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00245(__PlayerPrefsX_SetColorArray_0024callable12_0024259_63__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1, int arg2)
		{
			_0024from((Color[])arg0, arg1, arg2);
		}

		public static __PlayerPrefsX_SetValue_0024callable0_0024262_117__ Adapt(__PlayerPrefsX_SetColorArray_0024callable12_0024259_63__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_SetColorArray_0024callable12_0024259_63___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00245(from).Invoke;
		}
	}
}
