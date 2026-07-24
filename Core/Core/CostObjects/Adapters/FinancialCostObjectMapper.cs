/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Cost Object Mapper               Component : Interface adapters                      *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Mapper                                  *
*  Type     : FinancialCostObjectMapper                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for financial cost object mapper.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

namespace Empiria.Financial.CostObject.Adapters {

  /// <summary>Mapping methods for financial cost object.</summary>
  static public class FinancialCostObjectMapper {

    static internal FixedList<FinancialCostObjectDto> Map(FixedList<FinancialCostObject> list) {
      return new FixedList<FinancialCostObjectDto>(list.Select(x => Map(x)));
    }

    static internal FinancialCostObjectDto Map(FinancialCostObject o) {
      return new FinancialCostObjectDto {
        CostObjectTypeId = o.CostType.Id,
        UID = o.UID,
        ExternalCode = o.ExternalCode,
        Description = o.Description,
        StartDate = o.StartDate,
        EndDate = o.EndDate,
        Status = o.Status.ToString()
      };
    }

  } // class FinancialCostObjectMapper

} // Namespace Empiria.Financial.CostObjects.Adapters 
