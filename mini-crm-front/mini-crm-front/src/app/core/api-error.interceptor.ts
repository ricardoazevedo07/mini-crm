import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { ProblemDetails } from './api-models';

export const apiErrorInterceptor: HttpInterceptorFn = (request, next) =>
  next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      const problem: ProblemDetails = error.error && typeof error.error === 'object'
        ? { ...error.error, status: error.status }
        : {
            status: error.status,
            title: error.status === 0 ? 'API indisponível' : 'Erro inesperado',
            detail: error.status === 0
              ? 'Não foi possível conectar ao backend.'
              : 'O servidor não pôde concluir a operação.',
            code: 'internal_error',
          };

      return throwError(() => problem);
    }),
  );
