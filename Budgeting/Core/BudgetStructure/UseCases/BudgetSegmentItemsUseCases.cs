/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Use cases Layer                         *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Use case interactor class               *
*  Type     : BudgetSegmentItemsUseCases                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for budget segment items.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Services;

using Empiria.Budgeting.Adapters;
using Empiria.Budgeting.Data;

namespace Empiria.Budgeting.UseCases {

  /// <summary>Use cases for budget segment items.</summary>
  public class BudgetSegmentItemsUseCases : UseCase {

    #region Constructors and parsers

    protected BudgetSegmentItemsUseCases() {
      // no-op
    }

    static public BudgetSegmentItemsUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<BudgetSegmentItemsUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<BudgetSegmentItemDto> BudgetSegmentItemsByType(string segmentTypeUID) {
      Assertion.Require(segmentTypeUID, nameof(segmentTypeUID));

      var segmentType = BudgetSegmentType.Parse(segmentTypeUID);

      FixedList<BudgetSegmentItem> values = BudgetSegmentItemsDataService.SegmentItems(segmentType);

      return BudgetSegmentItemMapper.Map(values);
    }

    #endregion Use cases

  }  // class BudgetSegmentItemsUseCases

}  // namespace Empiria.Budgeting.UseCases
