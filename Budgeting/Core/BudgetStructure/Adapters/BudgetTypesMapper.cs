﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetTypesMapper                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetType instances to data transfer objects.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Budgeting.Adapters {

  /// <summary>Maps BudgetType instances to data transfer objects.</summary>
  static internal class BudgetTypesMapper {

    #region Mappers

    static internal FixedList<BudgetTypeDto> Map(FixedList<BudgetType> budgetTypes,
                                                 FixedList<Budget> budgets) {
      return budgetTypes.Select(x => Map(x, budgets)).ToFixedList();
    }


    static internal BudgetTypeDto Map(BudgetType budgetType, FixedList<Budget> budgets) {
      return new BudgetTypeDto {
        UID = budgetType.Name,
        Name = budgetType.DisplayName,
        SegmentTypes = BudgetSegmentTypesMapper.Map(budgetType.SegmentTypes),
        Budgets = BudgetMapper.Map(budgets.FindAll(x => x.BudgetType.Equals(budgetType)))
      };
    }

    #endregion Mappers

  }  // class BudgetTypesMapper

}  // namespace Empiria.Budgeting.Adapters
