import { Component } from '@angular/core';
import { PagedStockItems, StockItemLookup } from './Models/StockItems/stockItemLookup';
import { MainFeedService } from './services/main-feed.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { EditStockItemDialogComponent } from './edit-stock-item-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-main-feed',
  imports: [
    CommonModule,
    MatCardModule,
    MatGridListModule,
    MatButtonModule
  ],
  templateUrl: './main-feed.component.html',
  styleUrl: './main-feed.component.css'
})
export class MainFeedComponent {
  pagedStockItems: PagedStockItems<StockItemLookup> | undefined;
  pageNumber = 1;
  pageSize = 10;
  constructor(private mainFeedService: MainFeedService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadStockItems();
  }

  loadStockItems(): void {
    this.mainFeedService.getStockItems(this.pageNumber, this.pageSize).subscribe(
      (data: PagedStockItems<StockItemLookup>) => {
        this.pagedStockItems = data;
      },
      error => {
        console.error("Error fetching stock items ", error);
      }
    )
  }

  onPageChange(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.loadStockItems();
  }

  deleteStockItem(stockItemId: number): void {
    this.mainFeedService.deleteStockItem(stockItemId).subscribe(
      () => {
        this.pagedStockItems = {
          ...this.pagedStockItems,
          items: this.pagedStockItems?.items.filter(i => i.stockItemId !== stockItemId) || [],
          pageNumber: this.pagedStockItems?.pageNumber || 1,
          pageSize: this.pagedStockItems?.pageSize || 10,
          totalItems: this.pagedStockItems?.totalItems || 0
        };
      },
      error => {
        console.error("Error deleting stock item ", error);
      }
    )
  }

  openEditDialog(item: StockItemLookup): void {
    const dialogRef = this.dialog.open(EditStockItemDialogComponent, {
      width: '80vw',
      height: '90vh',
      maxWidth: '100vw',
      maxHeight: '100vh', 
      data: { item }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const index = this.pagedStockItems?.items.findIndex(i => i.stockItemId === result.id);
        if (index !== undefined && index !== -1 && this.pagedStockItems) {
          this.pagedStockItems.items[index] = result;
        }
      }
    })
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(EditStockItemDialogComponent, {
      width: '80vw',
      height: '90vh',
      maxWidth: '100vw',
      maxHeight: '100vh', 
      data: { item: {} }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.pagedStockItems?.items.push(result);
      }
    })
  }
}
