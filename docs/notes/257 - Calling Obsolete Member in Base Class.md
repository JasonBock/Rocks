I think when I look at when a member is obsolete, I also need to look to see if it's virtual (or override), because if it is, I'll end up doing a `base.` call on it, and that base definition may be obsolete.

Be careful of recursion!