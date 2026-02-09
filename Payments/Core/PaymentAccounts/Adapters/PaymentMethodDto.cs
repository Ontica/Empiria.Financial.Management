/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Output DTO                              *
*  Type     : PaymentMethodDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for PaymentMethod instances.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Adapters {

  /// <summary>Output DTO for PaymentMethod instances.</summary>
  public class PaymentMethodDto : NamedEntityDto {

    public PaymentMethodDto(PaymentMethod method) : base(method) {
      AccountRelated = method.AccountRelated;
    }

    public bool AccountRelated {
      get;
    }

    #region Mappers

    static public FixedList<PaymentMethodDto> Map(FixedList<PaymentMethod> methods) {
      return methods.Select(x => new PaymentMethodDto(x))
                    .ToFixedList();
    }

    #endregion Mappers

  }  // class PaymentMethodDto

}  // namespace Empiria.Payments.Adapters
