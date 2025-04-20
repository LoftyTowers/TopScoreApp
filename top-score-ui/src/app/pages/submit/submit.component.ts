import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WordService } from '../../services/word.service';
import { WordEntryModel } from '../../services/word.service';

@Component({
  selector: 'app-submit',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './submit.component.html',
  styleUrls: ['./submit.component.scss'],
})
export class SubmitComponent {
  sentence: string = '';
  success: boolean | null = null;
  validationErrors: string[] = [];
  savedWord: string = '';

  constructor(private wordService: WordService) {}


  submitSentence() {
    this.wordService.submitSentence(this.sentence).subscribe({
      next: (result) => {
        this.success = true;
        this.savedWord = result.word;
        this.sentence = '';
        this.validationErrors = [];
      },
      error: (err) => {
        this.success = false;
        this.savedWord = '';
        this.validationErrors = [];

        if (err.status === 400 && err.error?.errors) {
          this.validationErrors = err.error.errors;
        } else if (typeof err.error === 'string') {
          this.validationErrors.push(err.error);
        } else {
          this.validationErrors.push('An unexpected error occurred.');
        }
      }
    });
  }

}
