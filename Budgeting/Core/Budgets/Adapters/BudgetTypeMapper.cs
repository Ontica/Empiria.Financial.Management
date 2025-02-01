/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetTypesMapper                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetType instances to data transfer objects.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Adapters {

  /// <summary>Maps BudgetType instances to data transfer objects.</summary>
  static public class BudgetTypeMapper {

    #region Mappers

    static internal FixedList<BudgetTypeDto> Map(FixedList<BudgetType> budgetTypes,
                                                 FixedList<Budget> budgets) {
      return budgetTypes.Select(x => Map(x, budgets)).ToFixedList();
    }


    static public BudgetTypeDto Map(BudgetType budgetType, FixedList<Budget> budgets) {
      return new BudgetTypeDto {
        UID = budgetType.Name,
        Name = budgetType.DisplayName,
        Multiyear = budgetType.Multiyear,
        SegmentTypes = BudgetSegmentTypesMapper.Map(budgetType.SegmentTypes),
        Budgets = BudgetMapper.Map(budgets.FindAll(x => x.BudgetType.Equals(budgetType))),
        TransactionTypes = budgetType.GetTransactionTypes()
      };
    }

    #endregion Mappers

  }  // class BudgetTypesMapper

}  // namespace Empiria.Budgeting.Adapters
