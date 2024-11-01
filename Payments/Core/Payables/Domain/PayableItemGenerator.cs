/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Service Layer                           *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service                                 *
*  Type     : PayableServices                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Service to import payable itemsa and management from payableEntities.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


using System;
using System.Collections.Generic;
using Empiria.Financial;
using Empiria.Payments.Payables.Adapters;

namespace Empiria.Payments.Payables {

  /// <summary>Service to import payable itemsa and management from payableEntities.</summary>
  internal class PayableItemGenerator {

    #region Constructors and parsers

    private PayableItemGenerator() {
      // Required by Empiria Framework for all partitioned types.
    }


    internal PayableItemGenerator(Payable payable) {
      Assertion.Require(payable, nameof(payable));

      this.Payable = payable;
    }

    #endregion Constructors and parsers

    private Payable Payable {
      get; set;
    }

    #region Methods

    internal FixedList<PayableItem> Generate() {

      var entityItems = Payable.PayableEntity.Items;
      List<PayableItem> payableItems = new List<PayableItem>();

      foreach (var entityItem in entityItems) {
        PayableItemFields fields = TransformEntityItmeToPayableItem(this.Payable,entityItem);

        PayableItem payableItem = AddPayableItem(fields);

        payableItems.Add(payableItem);
      }

      return payableItems.ToFixedList();
    }

    #endregion Methods

    #region Helpers

    private PayableItem AddPayableItem(PayableItemFields fields) {
      PayableItem payableItem = this.Payable.CreateItem();

      payableItem.Update(fields);

      this.Payable.AddItem(payableItem);

      payableItem.Save();

      return payableItem;
    }


    private PayableItemFields TransformEntityItmeToPayableItem(Payable payable, IPayableEntityItem entityItem) {
      return new PayableItemFields {
        PayableUID = this.Payable.UID,
        BudgetAccountUID = entityItem.BudgetAccount.UID,
        ProductUID = entityItem.Product.UID,
        UnitUID = entityItem.Unit.UID,
        Description = entityItem.Description,
        Quantity = entityItem.Quantity,
        UnitPrice = entityItem.UnitPrice,
        CurrencyUID = "358626ea-3c2c-44dd-80b5-18017fe3927e", //Todo 
        ExchangeRate = 1m, //Todo
      };
    }

    #endregion Helpers

  }  // class PayableItemGenerator

}  // namespace Empiria.Payments.Payables
