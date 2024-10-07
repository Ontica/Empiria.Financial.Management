/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetSegmentItemMapper                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for budget segment items.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Budgeting.Adapters {

  /// <summary>Mapping methods for budget segment items.</summary>
  static internal class BudgetSegmentItemMapper {

    static internal FixedList<BudgetSegmentItemDto> Map(FixedList<BudgetSegmentItem> segmentItems) {
      return segmentItems.Select(x => Map(x)).ToFixedList();
    }

    static private BudgetSegmentItemDto Map(BudgetSegmentItem segmentItem) {
      BudgetSegmentItemDto dto = MapWithoutStructure(segmentItem);

      if (segmentItem.HasParent) {
        dto.Parent = MapWithoutStructure(segmentItem.Parent);
      }

      dto.Children = segmentItem.Children.Select(x => MapWithoutStructure(x))
                                         .ToFixedList();

      return dto;
    }

    static private BudgetSegmentItemDto MapWithoutStructure(BudgetSegmentItem segmentItem) {
      return new BudgetSegmentItemDto {
        UID = segmentItem.UID,
        Code = segmentItem.Code,
        Name = segmentItem.Name,
        Description = segmentItem.Description,
        Type = BudgetSegmentTypesMapper.MapWithoutStructure(segmentItem.BudgetSegmentType)
      };
    }

  }  // class BudgetSegmentItemMapper

}  // namespace Empiria.Budgeting.Adapters
