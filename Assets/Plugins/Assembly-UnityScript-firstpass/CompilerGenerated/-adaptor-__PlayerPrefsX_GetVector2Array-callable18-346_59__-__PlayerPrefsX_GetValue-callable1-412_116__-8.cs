using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompilerGenerated
{
	[Serializable]
	internal sealed class _0024adaptor_0024__PlayerPrefsX_GetVector2Array_0024callable18_0024346_59___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00248
	{
		protected __PlayerPrefsX_GetVector2Array_0024callable18_0024346_59__ _0024from;

		public _0024adaptor_0024__PlayerPrefsX_GetVector2Array_0024callable18_0024346_59___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00248(__PlayerPrefsX_GetVector2Array_0024callable18_0024346_59__ from)
		{
			_0024from = from;
		}

		public void Invoke(IList arg0, byte[] arg1)
		{
			_0024from((List<Vector2>)arg0, arg1);
		}

		public static __PlayerPrefsX_GetValue_0024callable1_0024412_116__ Adapt(__PlayerPrefsX_GetVector2Array_0024callable18_0024346_59__ from)
		{
			return new _0024adaptor_0024__PlayerPrefsX_GetVector2Array_0024callable18_0024346_59___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00248(from).Invoke;
		}
	}
}
