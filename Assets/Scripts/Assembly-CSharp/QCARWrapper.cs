public static class QCARWrapper
{
	private static IQCARWrapper sWrapper;

	public static IQCARWrapper Instance
	{
		get
		{
			if (sWrapper == null)
			{
				Create();
			}
			return sWrapper;
		}
	}

	public static void Create()
	{
		if (QCARRuntimeUtilities.IsQCAREnabled())
		{
			sWrapper = new QCARNativeWrapper();
		}
		else
		{
			sWrapper = new QCARNullWrapper();
		}
	}

	public static void SetImplementation(IQCARWrapper implementation)
	{
		sWrapper = implementation;
	}
}
