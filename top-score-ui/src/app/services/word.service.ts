import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface WordEntryModel {
  id: number;
  word: string;
  length: number;
  createdAt: string;
}

@Injectable({
  providedIn: 'root',
})
export class WordService {
  private apiUrl = 'http://localhost:5210/api/words';

  constructor(private http: HttpClient) {}

  submitSentence(sentence: string): Observable<WordEntryModel> {
    return this.http.post<WordEntryModel>(this.apiUrl, { sentence });
  }

  getWords(search = '', page = 1, pageSize = 10): Observable<{
    total: number;
    page: number;
    pageSize: number;
    items: WordEntryModel[];
  }> {
    const params = new URLSearchParams({
      search,
      page: page.toString(),
      pageSize: pageSize.toString()
    });

    return this.http.get<{
      total: number;
      page: number;
      pageSize: number;
      items: WordEntryModel[];
    }>(`${this.apiUrl}?${params.toString()}`);
  }
}
