import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { PagedStockItems, StockItemLookup } from '../Models/StockItems/stockItemLookup';

@Injectable({
  providedIn: 'root'
})
export class MainFeedService {
  private apiUrl = `${environment.apiUrl}/stockmanagement`;

  constructor(private http: HttpClient) { }

  getStockItems(pageNumber: number, pageSize: number): Observable<PagedStockItems<StockItemLookup>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

      return this.http.get<PagedStockItems<StockItemLookup>>(`${this.apiUrl}/stock-items`, { params });
  }

  healthCheck(): Observable<string> {
    return this.http.get(`${this.apiUrl}/health-check`, { responseType: 'text' });
  }

  updateStockItem(formData: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/update-stock-item`, formData);
  }

  deleteStockItem(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete-stock-item/${id}`);
  }
}
