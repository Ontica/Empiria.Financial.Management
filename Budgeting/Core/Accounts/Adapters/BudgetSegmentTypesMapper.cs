/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetSegmentTypesMapper                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetSegmentType instances to data transfer objects.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Budgeting.Adapters {

  /// <summary>Maps BudgetSegmentType instances to data transfer objects.</summary>
  static internal class BudgetSegmentTypesMapper {

    #region Mappers

    static internal FixedList<BudgetSegmentTypeDto> Map(FixedList<StandardAccountCategory> categories) {
      return categories.Select(x => Map(x)).ToFixedList();
    }


    static internal BudgetSegmentTypeDto Map(StandardAccountCategory category) {
      var dto = MapWithoutStructure(category);

      if (!category.Parent.IsEmptyInstance) {
        dto.ParentSegmentType = MapWithoutStructure(category.Parent);
        dto.ParentSegmentType.Name = category.Parent.Name;
      }

      if (category.HasChild) {
        dto.ChildrenSegmentType = MapWithoutStructure(category.Child);
        dto.ChildrenSegmentType.Name = category.Child.Name;
      }

      return dto;
    }

    static internal BudgetSegmentTypeDto MapWithoutStructure(StandardAccountCategory category) {
      return new BudgetSegmentTypeDto {
        UID = category.UID,
        Name = category.Name
      };
    }

    #endregion Mappers

  }  // class BudgetSegmentTypesMapper

}  // namespace Empiria.Budgeting.Adapters
