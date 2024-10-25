/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Interface adapters                      *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Fields Input DTO                        *
*  Type     : PayableLinkFields                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains fields in order to link payables with objects like                                    *
*             Bills or necesary documtents to pay.                                                           *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/



namespace Empiria.Payments.Payables.Adapters {

  /// <summary>Contains fields in order to link payables with objects like
  ///          Bills or necesary documtents to pay.  </summary>
  public class PayableLinkFields {

    #region Properties

    public string LinkTypeUID {
      get; set;
    } = string.Empty;


    public string PayableUID {
      get; set;
    } = string.Empty;


    public string LinkedObjectUID {
      get; set;
    } = string.Empty;


    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      Assertion.Require(LinkTypeUID, "Necesito el UID del tipo link correspondiente.");
      Assertion.Require(PayableUID, "Necesito UID de la solicitud de pago.");
      Assertion.Require(LinkedObjectUID, "Necesito UID del objeto con el cual ligar la solicitud de pago.");
        }

    #endregion Methods

  }  // class PayableLinkFields

}  // namespace Empiria.Payments.Payables.Adapters
