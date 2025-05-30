using System;
using System.Collections;
using UnityEngine;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_SetQuaternionArray_0024callable10_0024255_73___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00244
	{
		protected __PlayerPrefsX_SetQuaternionArray_0024callable10_0024255_73__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_SetQuaternionArray_0024callable10_0024255_73___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00244(__PlayerPrefsX_SetQuaternionArray_0024callable10_0024255_73__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1, int arg2)
		{
			_0024from((Quaternion[])arg0, arg1, arg2);
		}

		public static __PlayerPrefsX_SetValue_0024callable0_0024262_117__ Adapt(__PlayerPrefsX_SetQuaternionArray_0024callable10_0024255_73__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_SetQuaternionArray_0024callable10_0024255_73___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00244(from).Invoke;
		}
	}
}
