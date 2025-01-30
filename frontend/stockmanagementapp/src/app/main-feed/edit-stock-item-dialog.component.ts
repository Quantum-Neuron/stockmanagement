import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from "@angular/common";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MainFeedService } from "./services/main-feed.service";
import { stockItemCommand } from "./Models/StockItems/stockItemCommand";

@Component({
    selector: 'app-edit-stock-item-dialog',
    templateUrl: './edit-stock-item-dialog.component.html',
    styleUrls: ['./edit-stock-item-dialog.component.css'],
    standalone: true,
    imports: [
        CommonModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        ReactiveFormsModule
    ]
})
export class EditStockItemDialogComponent {
    editForm: FormGroup;
    primaryImage: File | null = null;
    secondaryImages: File[] = [];

    constructor(
        private fb: FormBuilder,
        private mainFeedService: MainFeedService,
        public dialogRef: MatDialogRef<EditStockItemDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
      ) {
        this.editForm = this.fb.group({
          make: [data.item.make, Validators.required],
          model: [data.item.model, Validators.required],
          modelYear: [data.item.modelYear, Validators.required],
          price: [data.item.price, Validators.required],
          kms: [data.item.kms, Validators.required],
          vin: [data.item.vin, Validators.required],
          registrationNumber: [data.item.registrationNumber, Validators.required],
        });
      }

      onPrimaryImageChange(event: any): void {
        this.primaryImage = event.target.files[0];
      }

      onSecondaryImagesChange(event: any): void {
        this.secondaryImages = Array.from(event.target.files);
      }

      onSave(): void {
        if (this.editForm.valid) {
            const formData = new FormData();
            formData.append('stockItemId', this.data.item.stockItemId.toString());
            formData.append('make', this.editForm.get('make')?.value);
            formData.append('model', this.editForm.get('model')?.value);
            formData.append('modelYear', this.editForm.get('modelYear')?.value);
            formData.append('price', this.editForm.get('price')?.value);
            formData.append('kms', this.editForm.get('kms')?.value);
            formData.append('vin', this.editForm.get('vin')?.value);
            formData.append('registrationNumber', this.editForm.get('registrationNumber')?.value);
      
            if (this.primaryImage) {
              formData.append('primaryImage', this.primaryImage);
            }
      
            this.secondaryImages.forEach((file, index) => {
              formData.append(`secondaryImages[${index}]`, file);
            });
      
            this.mainFeedService.updateStockItem(formData).subscribe(
              response => {
                this.dialogRef.close(response);
              },
              error => {
                console.error('Error updating stock item', error);
              }
            );
          }
      }

      onCancel(): void {
        this.dialogRef.close();
      }
}