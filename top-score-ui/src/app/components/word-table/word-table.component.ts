import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WordEntryModel } from '../../services/word.service';

@Component({
  selector: 'app-word-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './word-table.component.html',
  styleUrls: ['./word-table.component.scss']
})
export class WordTableComponent {
  @Input() words: WordEntryModel[] = [];

  @Input() totalCount: number = 0;
  @Input() page: number = 1;
  @Input() pageSize: number = 10;
  @Input() pageChanged: (page: number) => void = () => {};

  get maxPage(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  sortColumn: string = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  onSort(column: keyof WordEntryModel | 'length') {
    // Toggle direction if clicking same column, else reset to ascending
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    this.words.sort((a, b) => {
      let aValue: string | number;
      let bValue: string | number;

      if (column === 'length') {
        aValue = a.word.length;
        bValue = b.word.length;
      } else if (column === 'createdAt') {
        aValue = new Date(a.createdAt).getTime();
        bValue = new Date(b.createdAt).getTime();
      } else {
        aValue = a[column];
        bValue = b[column];
      }

      if (typeof aValue === 'string' && typeof bValue === 'string') {
        return this.sortDirection === 'asc'
          ? aValue.localeCompare(bValue)
          : bValue.localeCompare(aValue);
      }

      return this.sortDirection === 'asc'
        ? (aValue as number) - (bValue as number)
        : (bValue as number) - (aValue as number);
    });
  }

  getPageButtons(): number[] {
    const pages: number[] = [];
    const max = this.maxPage;
    const start = Math.max(1, this.page - 2);
    const end = Math.min(max, this.page + 2);

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    return pages;
  }


  changePage(p: number) {
    if (p !== this.page) {
      this.pageChanged(p);
    }
  }
}
