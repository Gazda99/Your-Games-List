using System;
using System.Collections.Generic;
using YGL.API.Errors;
using YGL.API.Exceptions;

namespace YGL.API.Validation; 

public static class ValidationUrl {
    private const char Delimiter = ',';
    private const char Range = '-';
    public static int MaxCount { get; set; } = 100;

    public static bool TryParseInt(string url, IErrorList errorList, out List<int> ids) {
        try {
            //check for null or empty
            if (string.IsNullOrEmpty(url))
                throw new CannotParseIdListInUriException(CannotParseIdListInUriException.BadValue);

            ids = new List<int>();

            foreach (string split in url.Split(Delimiter)) {
                //check for range of ids
                if (split.Contains(Range)) {
                    string[] s = split.Split(Range);

                    if (s[0] == string.Empty) {
                        if (s[1] == string.Empty)
                            throw new CannotParseIdListInUriException(CannotParseIdListInUriException.BadValue);
                        else
                            throw new CannotParseIdListInUriException(CannotParseIdListInUriException.NonPositiveId);
                    }

                    int a = ParseSingleIdInt(s[0]); //from
                    int b = ParseSingleIdInt(s[1]); //to

                    //add every id in range
                    for (; a <= b; a++)
                        AddInt(ref ids, a);
                }
                else {
                    int id = ParseSingleIdInt(split);
                    AddInt(ref ids, id);
                }
            }
        }
        catch (Exception ex) {
            if (ex.Message.Contains(CannotParseIdListInUriException.NonPositiveId))
                errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CannotParseIdsInUrlNonPositiveId);
            else if (ex.Message.Contains(CannotParseIdListInUriException.TooManyItems))
                errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CannotParseIdsInUrlTooManyIds);
            else
                errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CannotParseIdsInUrlBadValue);

            ids = null;
            return false;
        }

        return true;
    }

    public static bool TryParseLong(string url, IErrorList errorList, out List<long> ids) {
        try {
            //check for null or empty
            if (string.IsNullOrEmpty(url))
                throw new CannotParseIdListInUriException(CannotParseIdListInUriException.BadValue);

            ids = new List<long>();

            foreach (string split in url.Split(Delimiter)) {
                //check for range of ids
                if (split.Contains(Range)) {
                    string[] s = split.Split(Range);

                    if (s[0] == string.Empty) {
                        if (s[1] == string.Empty)
                            throw new CannotParseIdListInUriException(CannotParseIdListInUriException.BadValue);
                        else
                            throw new CannotParseIdListInUriException(CannotParseIdListInUriException.NonPositiveId);
                    }

                    long a = ParseSingleIdLong(s[0]); //from
                    long b = ParseSingleIdLong(s[1]); //to

                    //add every id in range
                    for (; a <= b; a++)
                        AddLong(ref ids, a);
                }
                else {
                    long id = ParseSingleIdLong(split);
                    AddLong(ref ids, id);
                }
            }
        }
        catch (Exception ex) {
            if (ex.Message.Contains(CannotParseIdListInUriException.NonPositiveId))
                errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CannotParseIdsInUrlNonPositiveId);
            else if (ex.Message.Contains(CannotParseIdListInUriException.TooManyItems))
                errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CannotParseIdsInUrlTooManyIds);
            else
                errorList.AddErrors<ApiErrors, ApiErrorCodes>(ApiErrorCodes.CannotParseIdsInUrlBadValue);

            ids = null;
            return false;
        }

        return true;
    }


    private static void AddInt(ref List<int> ids, int id) {
        if (ids.Count >= MaxCount)
            throw new CannotParseIdListInUriException(CannotParseIdListInUriException.TooManyItems);

        if (id <= 0)
            throw new CannotParseIdListInUriException(CannotParseIdListInUriException.NonPositiveId);

        if (ids.Contains(id))
            return;

        ids.Add(id);
    }

    private static void AddLong(ref List<long> ids, long id) {
        if (ids.Count >= MaxCount)
            throw new CannotParseIdListInUriException(CannotParseIdListInUriException.TooManyItems);

        if (id <= 0)
            throw new CannotParseIdListInUriException(CannotParseIdListInUriException.NonPositiveId);

        if (ids.Contains(id))
            return;

        ids.Add(id);
    }

    private static int ParseSingleIdInt(string value) {
        try {
            int id = int.Parse(value.Trim());
            return id;
        }
        catch (Exception) {
            throw new CannotParseIdListInUriException(CannotParseIdListInUriException.BadValue);
        }
    }

    private static long ParseSingleIdLong(string value) {
        try {
            long id = long.Parse(value.Trim());
            return id;
        }
        catch (Exception) {
            throw new CannotParseIdListInUriException(CannotParseIdListInUriException.BadValue);
        }
    }
}