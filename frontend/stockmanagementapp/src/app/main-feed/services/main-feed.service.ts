import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { StockItem } from '../Models/stockItem';

@Injectable({
  providedIn: 'root'
})
export class MainFeedService {
  private apiUrl = `${environment.apiUrl}/stockmanagementcontroller`;

  constructor(private http: HttpClient) { 
    getStockItemsAsync(): Observable<StockItem[]>
  }
}
