namespace RimWorldBingoMod.Objectives
{
	public class ObjectiveCandidate<T>
	{
		public T value;
		public int degree = 0;

		public double weight = 1.0;
		public double multiplier = 1.0;
		public int offset = 0;

		public static implicit operator ObjectiveCandidate<T>(T value)
		{
			return new ObjectiveCandidate<T>()
			{
				value = value,
				weight = 1.0,
				multiplier = 1.0
			};
		}
	}
}