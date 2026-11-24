namespace YomiLite.Core
{
   /* DecisionChoice — логический тип с вариантами StartMove, Continue, FreeCancel, ReactWithDI, Burst;
    * у каждого варианта есть только допустимые для него параметры.
    * Например, StartMove(Jump, Direction, Strength) и ReactWithDI(Direction).
    * Нельзя разрешать несовместимые варианты одной реакцией без отдельного правила. */
   public class DecisionChoice
   {

   }
}
