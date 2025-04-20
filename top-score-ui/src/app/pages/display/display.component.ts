import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WordService, WordEntryModel } from '../../services/word.service';
import { WordTableComponent } from '../../components/word-table/word-table.component';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-display',
  standalone: true,
  imports: [CommonModule, FormsModule, WordTableComponent],
  templateUrl: './display.component.html',
  styleUrls: ['./display.component.scss']
})
export class DisplayComponent implements OnDestroy {
  words: WordEntryModel[] = [];
  totalCount: number = 0;
  page: number = 1;
  pageSize: number = 10;
  search: string = '';
  error: boolean = false;

  pageSizeOptions = [5, 10, 25, 50];
  private searchSubject = new Subject<string>();

  constructor(private wordService: WordService) {
    this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe(() => {
        this.page = 1;
        this.loadWords();
      });
  }

  ngOnInit() {
    this.loadWords();
  }

  ngOnDestroy() {
    this.searchSubject.unsubscribe();
  }

  onSearchChange() {
    this.searchSubject.next(this.search);
  }

  loadWords() {
    this.wordService.getWords(this.search, this.page, this.pageSize).subscribe({
      next: (res) => {
        this.words = res.items;
        this.totalCount = res.total;
        this.page = res.page;
        this.pageSize = res.pageSize;
        this.error = false;
      },
      error: () => {
        this.error = true;
      }
    });
  }

  onPageSizeChange() {
    this.page = 1;
    this.loadWords();
  }

  changePage(newPage: number) {
    this.page = newPage;
    this.loadWords();
  }
}
