﻿using Barcode.BarcodeCLI;

namespace Barcode.Controller.Commands.UserCommands
{
    class ProductSetCreditState : ProductSetCommand
    {
        private bool credit;

        public ProductSetCreditState(string[] command, bool canBeBoughtOnCredit, IBarcodeCLI barcodeCli, IBarcodeSystem barcodeSystem) : base(command, barcodeCli, barcodeSystem)
        {
            credit = canBeBoughtOnCredit;
        }

        public override void Execute()
        {
            if (!TryGetProduct()) return;

            product.CanBeBoughtOnCredit = credit;
            barcodeCli.DisplayProductOnCreditChange(product);
            Succeeded = true;
            
            base.Execute();
        }

        public override void Undo()
        {
            if (!Succeeded) return;

            product.CanBeBoughtOnCredit = !credit;
            Undone = true;
            
            base.Undo();
        }
    }
}