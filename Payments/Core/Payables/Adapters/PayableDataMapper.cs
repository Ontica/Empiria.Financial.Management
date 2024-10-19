/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Mapper                                  *
*  Type     : Payable}DataMapper                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for payable data objects and related types.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Xml.Linq;

namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Provides data mapping services for payable data objects and related types.</summary>
  internal class PayableDataMapper {

    static internal PayableDataDto Map(Payable payable) {
      return new PayableDataDto {
        UID = payable.UID,
        Type = new NamedEntityDto(payable.PayableType.UID, payable.PayableType.DisplayName),
        
        Status = new NamedEntityDto(payable.Status.ToString(), payable.Status.GetName())
      };

    }

    static internal PayableEntityDto Map() {

      return new PayableEntityDto {
        UID = "",
        Type = "",
        Name = "",
        Items = MapPayableEntityItemsDto()
      };

    }

    static internal FixedList<PayableEntityItemDto> MapPayableEntityItemsDto() {

      return new List<PayableEntityItemDto>().ToFixedList();
    }



  } // class PayableMapper

} // namespace Empiria.Payments.Payables.Adapters