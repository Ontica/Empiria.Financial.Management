/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Adapters Layer                          *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Mapping class                           *
*  Type     : BudgetSegmentTypesMapper                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Maps BudgetSegmentType instances to data transfer objects.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Adapters {

  /// <summary>Maps BudgetSegmentType instances to data transfer objects.</summary>
  static internal class BudgetSegmentTypesMapper {

    #region Mappers

    static internal FixedList<BudgetSegmentTypeDto> Map(FixedList<BudgetAccountSegmentType> segmentTypes) {
      return segmentTypes.Select(x => Map(x)).ToFixedList();
    }


    static internal BudgetSegmentTypeDto Map(BudgetAccountSegmentType segmentType) {
      var dto = MapWithoutStructure(segmentType);

      if (segmentType.HasParentSegmentType) {
        dto.ParentSegmentType = MapWithoutStructure(segmentType.ParentSegmentType);
        dto.ParentSegmentType.Name = segmentType.ParentSegmentType.AsParentName;
      }

      if (segmentType.HasChildrenSegmentType) {
        dto.ChildrenSegmentType = MapWithoutStructure(segmentType.ChildrenSegmentType);
        dto.ChildrenSegmentType.Name = segmentType.ChildrenSegmentType.AsChildrenName;
      }

      return dto;
    }

    static internal BudgetSegmentTypeDto MapWithoutStructure(BudgetAccountSegmentType segmentType) {
      return new BudgetSegmentTypeDto {
        UID = segmentType.UID,
        Name = segmentType.DisplayName
      };
    }

    #endregion Mappers

  }  // class BudgetSegmentTypesMapper

}  // namespace Empiria.Budgeting.Adapters
