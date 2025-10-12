namespace SchoolTripApi.Domain.GradeLevelAggregate;

public static class BrazilianSchoolGradeSystem
{
    // 0-1 years old: 'Berçário'
    public static readonly string Ei1 = "EI1";

    // 1-2 years old: 'Maternal I', also known as 'G2' in private schools
    public static readonly string Ei2 = "EI2";

    // 2-3 years old: 'Maternal II', also known as 'G3' in private schools
    public static readonly string Ei3 = "EI3";

    // 3-4 years old: 'Pré-Escola I', also known as 'G4' in private schools
    public static readonly string Ei4 = "EI4";

    // 4-5 years old: 'Pré-Escola II', also known as 'G5' in private schools
    public static readonly string Ei5 = "EI5";

    // 5-6 years old: 'Ensino Fundamental (Anos Iniciais): 1° ano'
    public static readonly string Efai1 = "EFAI1";

    // 6-7 years old: 'Ensino Fundamental (Anos Iniciais): 2° ano'
    public static readonly string Efai2 = "EFAI2";

    // 7-8 years old: 'Ensino Fundamental (Anos Iniciais): 3° ano' 
    public static readonly string Efai3 = "EFAI3";

    // 8-9 years old: 'Ensino Fundamental (Anos Iniciais): 4° ano'
    public static readonly string Efai4 = "EFAI4";

    // 9-10 years old: 'Ensino Fundamental (Anos Iniciais): 5° ano'
    public static readonly string Efai5 = "EFAI5";

    // 10-11 years old: 'Ensino Fundamental (Anos Finais): 6° ano'
    public static readonly string Efai6 = "EFAI6";

    // 11-12 years old: 'Ensino Fundamental (Anos Finais): 7° ano'
    public static readonly string Efai7 = "EFAI7";

    // 12-13 years old: 'Ensino Fundamental (Anos Finais): 8° ano'
    public static readonly string Efai8 = "EFAI8";

    // 13-14 years old: 'Ensino Fundamental (Anos Finais): 9° ano'
    public static readonly string Efai9 = "EFAI9";

    // 14-15 years old: 'Ensino Médio: 1° ano'
    public static readonly string Em1 = "EM1";

    // 15-16 years old: 'Ensino Médio: 2° ano'
    public static readonly string Em2 = "EM2";

    // 16-17 years old: 'Ensino Médio: 3° ano'
    public static readonly string Em3 = "EM3";

    public static List<string> GetBrazilianSchoolGrades()
    {
        return
        [
            Ei1, Ei2, Ei3, Ei4, Ei5,
            Efai1, Efai2, Efai3, Efai4, Efai5, Efai6, Efai7, Efai8, Efai9,
            Em1, Em2, Em3
        ];
    }
}