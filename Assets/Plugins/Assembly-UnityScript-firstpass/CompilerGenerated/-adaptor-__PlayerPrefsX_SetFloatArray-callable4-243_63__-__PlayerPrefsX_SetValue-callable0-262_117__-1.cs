using System;
using System.Collections;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_SetFloatArray_0024callable4_0024243_63___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00241
	{
		protected __PlayerPrefsX_SetFloatArray_0024callable4_0024243_63__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_SetFloatArray_0024callable4_0024243_63___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00241(__PlayerPrefsX_SetFloatArray_0024callable4_0024243_63__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1, int arg2)
		{
			_0024from((float[])arg0, arg1, arg2);
		}

		public static __PlayerPrefsX_SetValue_0024callable0_0024262_117__ Adapt(__PlayerPrefsX_SetFloatArray_0024callable4_0024243_63__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_SetFloatArray_0024callable4_0024243_63___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00241(from).Invoke;
		}
	}
}
