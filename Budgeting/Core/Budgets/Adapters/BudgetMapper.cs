/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetMapper                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps Budget instances to data transfer objects.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Adapters {

  /// <summary>Maps Budget instances to data transfer objects.</summary>
  static internal class BudgetMapper {

    static internal FixedList<BudgetDto> Map(FixedList<Budget> budgets) {
      return budgets.Select(x => Map(x)).ToFixedList();
    }

    #region Helpers

    static private BudgetDto Map(Budget budget) {
      return new BudgetDto {
        UID = budget.UID,
        Name = budget.Name,
        Year = budget.Year,
        Type = new NamedEntityDto(budget.BudgetType.UID, budget.BudgetType.DisplayName),
        TransactionTypes = budget.AvailableTransactionTypes.MapToNamedEntityList()
      };
    }

    #endregion Helpers

  }  // class BudgetMapper

}  // namespace Empiria.Budgeting.Adapters
